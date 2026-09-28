using Microsoft.AspNetCore.Mvc;

namespace programmingPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ChallengeController : ControllerBase
{
    public static readonly List<Challenge> challenges = new()
    {
        new Challenge
        {
            Id = 1,
            Title = "Suma",
            Difficulty = "Easy",
            HighScore = 100,
            Status = true
        }  
    };
    private readonly ILogger<ChallengeController> _logger;

    public ChallengeController(ILogger<ChallengeController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Challenge newChallenge)
    {
        try
        {
            bool idExists = challenges.Any(C => C.Id == newChallenge.Id);

            if (idExists)
            {
                return Conflict("El id que intentas ingresar ya existe.");
            }

            bool titleExists = challenges.Any(C => C.Title == newChallenge.Title);

            if (titleExists)
            {
                return Conflict("El titulo que intentas ingresar ya existe.");
            }

            challenges.Add(newChallenge);
            return Ok("Desafio creado exitosamente");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }
}