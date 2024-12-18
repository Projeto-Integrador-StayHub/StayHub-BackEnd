using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class ScraperController : ControllerBase
{
    [HttpPost("start-scraping")]
    public IActionResult StartScraping()
    {
        try
        {
            // Step 1: Install Python dependencies
            ProcessStartInfo installDependencies = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "-m pip install -r Scraper/requirements.txt",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(installDependencies))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
                using (StreamReader reader = process.StandardError)
                {
                    string error = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.WriteLine($"Error installing dependencies: {error}");
                        return StatusCode(500, $"Error installing dependencies: {error}");
                    }
                }
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Debug.WriteLine($"Process exited with code {process.ExitCode} while installing dependencies.");
                    return StatusCode(500, $"Process exited with code {process.ExitCode} while installing dependencies.");
                }
            }

            // Step 2: Install Playwright browsers
            ProcessStartInfo installBrowsers = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "-m playwright install chromium",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(installBrowsers))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
                using (StreamReader reader = process.StandardError)
                {
                    string error = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.WriteLine($"Error installing Playwright browsers: {error}");
                        return StatusCode(500, $"Error installing Playwright browsers: {error}");
                    }
                }
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Debug.WriteLine($"Process exited with code {process.ExitCode} while installing Playwright browsers.");
                    return StatusCode(500, $"Process exited with code {process.ExitCode} while installing Playwright browsers.");
                }
            }

            // Step 3: Execute the scraping script
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "Scraper/booking_scraper.py",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
                using (StreamReader reader = process.StandardError)
                {
                    string error = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.WriteLine($"Error executing script: {error}");
                        return StatusCode(500, $"Error executing script: {error}");
                    }
                }
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Debug.WriteLine($"Process exited with code {process.ExitCode} while executing script.");
                    return StatusCode(500, $"Process exited with code {process.ExitCode} while executing script.");
                }
            }

            return Ok("Scraping completed successfully!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
