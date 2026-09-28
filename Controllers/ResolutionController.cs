using Microsoft.AspNetCore.Mvc;

namespace programmingPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ResolutionController : ControllerBase
{
    private static readonly List<Resolution> resolutions = new()
    {
        new Resolution
        {
            Id = 1,
            ParticipantId = 1,
            ChallengeId1 = 1,
            ObtainedPoints = 50,
            DeliverDate = DateOnly.Parse("2026-12-19")
        }
    };
    private readonly ILogger<ResolutionController> _logger;

    public ResolutionController(ILogger<ResolutionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Resolution newResolution)
    {
        try
        {
            bool idExists = resolutions.Any(R => R.Id == newResolution.Id);

            if (idExists)
            {
                return Conflict("El id que intentas ingresar ya existe.");
            }

            var participant = ParticipantController.participants.FirstOrDefault(P => P.Id == newResolution.ParticipantId);

            if (participant is null)
            {
                return Conflict("El participante con ese id no existe.");
            }

            var participantValidation = resolutions.FirstOrDefault(P => P.ParticipantId == newResolution.ParticipantId);

            if (participantValidation is not null)
            {
                return Conflict("El un participante no puede repetir un desafio.");
            }

            var challenge = ChallengeController.challenges.FirstOrDefault(C => C.Id == newResolution.ChallengeId1);

            if (challenge is null)
            {
                return Conflict("El desafio con ese id no existe.");
            }

            if (challenge.Status == false)
            {
                return Conflict("El desafio se encuentra inactivo");
            }

            if (newResolution.ObtainedPoints > challenge.HighScore)
            {
                return Conflict("Los puntos obtenidos no pueden superar la puntuacion maxima del desafio.");
            }

            resolutions.Add(newResolution);
            return Ok("Resolucion creada exitosamente.");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }

    [HttpGet("resolucion/Participante")]
    public IActionResult GetByParticipant(int participantId)
    {
        try
        {
            if (resolutions.Count == 0)
            {
                return NotFound("No hay resoluciones registradas aun.");
            }

            var participantResolutions = resolutions.Where(R => R.ParticipantId == participantId)
                                                    .ToList();
            
            if (participantResolutions.Count == 0)
            {
                return NotFound("El participante no existe o aun no realizo desafios.");
            }

            return Ok(participantResolutions);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }

    [HttpGet("Participante/Puntuacion")]
    public IActionResult GetByObtainedPoints(int points)
    {
        try
        {    
            if (resolutions.Count == 0)
            {
                return NotFound("No hay resoluciones registradas aun.");
            }

            var participantPoints = resolutions.Where(R => R.ObtainedPoints > points).ToList();

            if (participantPoints.Count == 0)
            {
                return NotFound($"No hay participantes que superen {points} puntos.");
            }

            return Ok(participantPoints);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }

    [HttpGet("Promedio")]
    public IActionResult GetAverage(int challengeId)
    {
        try
        {
            if (ChallengeController.challenges.Count == 0)
            {
                return NotFound("No hay desafios registrados aun.");
            }

            var average = resolutions.Where(R => R.ChallengeId1 == challengeId)
                                    .Average(C => C.ObtainedPoints);
            
            return Ok(average);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }
    
    [HttpGet("MaxPuntuacion")]
    public IActionResult GetMaxPoints()
    {
        try
        {
            if (ParticipantController.participants.Count == 0)
            {
                return NotFound("No hay participantes registrados aun.");
            }

            var maxPoints = resolutions.Max(C => C.ObtainedPoints);
            Resolution participantMaxPoint = resolutions.FirstOrDefault(R => R.ObtainedPoints == maxPoints);
            var prpr = ParticipantController.participants.FirstOrDefault(P => P.Id == participantMaxPoint.ParticipantId);

            return Ok(prpr);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }
}