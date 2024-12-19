using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CrmApp.Models;

namespace CrmApp.Pages.Employees
{
    public class Employee_PageModel : PageModel
    {
        private static List<Employee> _people = new List<Employee>();
        private readonly ILogger<Employee_PageModel> _logger;

        public Employee_PageModel(ILogger<Employee_PageModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EditIndex { get; set; }

        public List<Employee> People => _people;

        public void OnGet()
        {
        }

        // Handle new employee submission
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adding new employee: {@Employee}", Employee);
                _people.Add(Employee);
                
                // Reset form
                Employee = new Employee();
                ModelState.Clear(); // Clear the ModelState to prevent carrying over old data
                return RedirectToPage(); // Redirect to refresh the page
            }

            return Page(); // Return to the same page if validation fails
        }

        // Handle update for existing employee
        public IActionResult OnPostUpdate()
        {
            if (ModelState.IsValid && EditIndex.HasValue)
            {
                _logger.LogInformation("Updating employee at index {Index}", EditIndex.Value);
                _people[EditIndex.Value] = Employee; // Update employee
                Employee = new Employee(); // Reset the form
                EditIndex = null; // Reset edit index
                ModelState.Clear(); // Clear the ModelState to prevent carrying over old data
                return RedirectToPage(); // Redirect to refresh the page
            }

            return Page(); // Return to the same page if validation fails
        }

        public IActionResult OnPostEdit(int index)
        {
            if (index >= 0 && index < _people.Count)
            {
                Employee = _people[index];
                EditIndex = index; // Set the edit index
                _logger.LogInformation("Editing employee at index {Index}", index); // Log the edit action
                return Page();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int index)
        {
            if (index >= 0 && index < _people.Count)
            {
                _logger.LogInformation("Deleting employee at index {Index}", index); // Log the delete action
                _people.RemoveAt(index);
            }
            return RedirectToPage();
        }
    }
}
