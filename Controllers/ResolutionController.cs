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

        var participantValidation = resolutions.Any(P => P.ParticipantId == newResolution.ParticipantId && P.ChallengeId1 == newResolution.ChallengeId1);

        if (!participantValidation)
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
}