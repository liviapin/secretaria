using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Secretaria.DataTransfer.Admin.Requests;
using Secretaria.Dominio.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inicializa uma nova instância do controlador de autenticação.
    /// </summary>
    /// <param name="userManager">Gerenciador de usuários do Identity.</param>
    /// <param name="signInManager">Gerenciador de login do Identity.</param>
    /// <param name="configuration">Configuração da aplicação para acesso a settings.</param>
    public AuthController(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager,
                          IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Autentica o usuário e retorna um token JWT válido se as credenciais estiverem corretas.
    /// </summary>
    /// <param name="request">Dados de login contendo e-mail e senha.</param>
    /// <returns>Token JWT para acesso autenticado.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Senha))
            return Unauthorized("Credenciais inválidas");

        var roles = await _userManager.GetRolesAsync(user);
        var token = GerarJwtToken(user, roles);

        return Ok(new { token });
    }

    /// <summary>
    /// Registra um novo usuário com perfil de administrador.
    /// </summary>
    /// <param name="request">Dados do administrador a ser cadastrado.</param>
    /// <returns>Mensagem de sucesso ou erros de validação.</returns>
    [HttpPost("cadastrar-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegistrarAdmin([FromBody] RegisterAdminRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Senha);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok("Administrador cadastrado com sucesso.");
    }

    /// <summary>
    /// Gera um token JWT para o usuário autenticado contendo suas claims e roles.
    /// </summary>
    /// <param name="user">Usuário autenticado.</param>
    /// <param name="roles">Lista de roles associadas ao usuário.</param>
    /// <returns>Token JWT codificado.</returns>
    private string GerarJwtToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
