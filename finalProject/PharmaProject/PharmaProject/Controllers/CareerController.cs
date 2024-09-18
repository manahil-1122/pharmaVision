using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaProject.Models;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace PharmaProject.Controllers
{
    public class CareerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CareerController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            // Fetch active resumes
            var activeResumes = _context.resume.Where(r => !r.IsPending).ToList();

            // If the number of active resumes is below the limit, move pending resumes to active
            int remainingSlots = 5 - activeResumes.Count;

            if (remainingSlots > 0)
            {
                var pendingResumes = _context.resume
                    .Where(r => r.IsPending)
                    .Take(remainingSlots)
                    .ToList();

                foreach (var resume in pendingResumes)
                {
                    resume.IsPending = false; // Move to active
                }

                _context.SaveChanges();
                activeResumes.AddRange(pendingResumes);
            }

            return View(activeResumes);
        }


        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            // Get the logged-in user's username
            var loggedInUsername = User.Identity.Name;

            // Retrieve the user from the database using the username
            var user = _context.Clients.FirstOrDefault(u => u.Username == loggedInUsername);

            if (user != null)
            {
                // Get the user's ID
                var userId = user.Id;

                // Check if there is a success message for the current user
                var successMessageKey = $"SuccessMessage_{userId}";
                if (TempData[successMessageKey] != null)
                {
                    ViewBag.SuccessMessage = TempData[successMessageKey];
                }
            }

            return View();
        }



        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Create(ApplyViewModal apps)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user's username
                var loggedInUsername = User.Identity.Name;

                // Retrieve the user from the database using the username
                var user = _context.Clients.FirstOrDefault(u => u.Username == loggedInUsername);

                if (user != null)
                {
                    // Check the number of existing active submissions
                    var totalSubmissions = _context.resume.Count(r => !r.IsPending);

                    string filename = "";
                    if (apps.UploadedFile != null)
                    {
                        string uploadedFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        filename = Guid.NewGuid().ToString() + '_' + apps.UploadedFile.FileName;
                        string filePath = Path.Combine(uploadedFolder, filename);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            apps.UploadedFile.CopyTo(fileStream);
                        }
                    }

                    // Create a new Apply object and set its properties
                    Apply a = new Apply
                    {
                        FullName = apps.FullName,
                        Email = apps.Email,
                        PhoneNo = apps.PhoneNo,
                        Degree = apps.Degree,
                        InstName = apps.InstName,
                        Skills = apps.Skills,
                        WorkExp = apps.WorkExp,
                        UploadedFilePath = filename,
                        IsPending = totalSubmissions >= 5, // Mark as pending if the limit is reached
                        UserId = user.Id // Associate the application with the logged-in user's ID
                    };


                    // Send confirmation email using SMTP
                    string username = apps.FullName;
                    string emailMessage = $"Dear {username},\n\n" +
                        "Thank you for submitting your CV at Pharma Care.\n" +
                        "We have successfully received your resume and our team will be reviewing it shortly.\n\n" +
                        "If your qualifications match the requirements of the position, we will contact you to discuss the next steps. \nWe appreciate your interest in joining our team and wish you the best of luck." +
                        "\nBest Regards,\nPharma Care.";

                    EmailSender emailSender = new EmailSender();
                    emailSender.SendEmailViaSmtp(apps.Email, "Resume Received", emailMessage);



                    // Save the new application to the database
                    _context.resume.Add(a);
                    _context.SaveChanges();

                    // Show a success message to the user
                    TempData["SuccessMessage"] = a.IsPending
                        ? "Application limit reached. Your application is pending."
                        : "Form Submitted Successfully!";

                    return RedirectToAction("Create");
                }
            }

            // If something goes wrong, return the view with the form data
            return View(apps);
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var resume = _context.resume.FirstOrDefault(r => r.Id == id);
            if (resume != null)
            {
                // Delete the resume
                _context.resume.Remove(resume);
                _context.SaveChanges();

                // After deletion, check if there are any pending resumes and move them to active
                var pendingResume = _context.resume.FirstOrDefault(r => r.IsPending);
                if (pendingResume != null)
                {
                    pendingResume.IsPending = false; // Move to active
                    _context.SaveChanges();

                    // Save the message in TempData with a specific key using the user's ID
                    TempData[$"SuccessMessage_{pendingResume.UserId}"] = "Your CV is now active and has been sent to the admin.";
                }
            }


            return RedirectToAction("Index");
        }

    }

}