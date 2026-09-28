using Microsoft.AspNetCore.Mvc;

namespace programmingPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantController : ControllerBase
{
    public static readonly List<Participant> participants = new()
    {
        new Participant
        {
            Id = 1,
            Name = "Pepe",
            Email = "elpepe@gmail.com",
            Level = 1
        }
    };
    private readonly ILogger<ParticipantController> _logger;

    public ParticipantController(ILogger<ParticipantController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Participant newParticipant)
    {
        try
        {
            bool idExists = participants.Any(P => P.Id == newParticipant.Id);

            if (idExists)
            {
                return Conflict("El id que intentas ingresar ya existe.");
            }

            bool emailExists = participants.Any(P => P.Email == newParticipant.Email);

            if (emailExists)
            {
                return Conflict("El email que intentas ingresar ya existe.");
            }

            participants.Add(newParticipant);
            return Ok("Participante creado exitosamente.");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ocurrio un error en el servidor: {ex.Message}");
        }
    }
}