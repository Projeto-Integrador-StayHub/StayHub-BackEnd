using Microsoft.AspNetCore.Mvc;
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
            ProcessStartInfo installDependencies = new ProcessStartInfo();
            installDependencies.FileName = "python";
            installDependencies.Arguments = "-m pip install -r Scraper/requirements.txt";
            installDependencies.UseShellExecute = false;
            installDependencies.RedirectStandardOutput = true;
            installDependencies.RedirectStandardError = true;

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
                        Debug.WriteLine($"Error instalando as dependencias: {error}");
                        return StatusCode(500, $"Error instalando as dependencias: {error}");
                    }
                }
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = "Scraper/booking_scraper.py";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

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
                        Debug.WriteLine(error);
                        return StatusCode(500, $"Error executando script: {error}");
                    }
                }
            }

            return Ok("Scraping realizado com sucesso!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}