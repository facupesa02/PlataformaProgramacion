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

    [HttpGet("Activos")]
    public IActionResult GetActives()
    {
        try
        {
            if (challenges.Count == 0)
            {
                return NotFound("No hay desafios registrados aun.");
            }

            var activeChallenges = challenges.Where(C => C.Status == true).ToList();
            
            if (activeChallenges.Count == 0)
            {
                return NotFound("No hay desafios activos.");
            }

            return Ok(activeChallenges);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }

    [HttpGet("Dificultad")]
    public IActionResult GetByDifficulty(string difficulty)
    {
        try
        {
            if (challenges.Count == 0)
            {
                return NotFound("No hay desafios registrados aun.");
            }

            var challengesDifficulty = challenges.Where(C => C.Difficulty == difficulty).ToList();

            if(challengesDifficulty.Count == 0)
            {
                return NotFound("No hay desafios con dicha dificultad.");
            }

            return Ok(challengesDifficulty);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }
}