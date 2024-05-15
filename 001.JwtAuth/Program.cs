using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["JWT:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // is issuer required
            ValidateIssuer = true,
            // issuer
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            // is audience required
            ValidateAudience = true,
            // audience
            ValidAudience = builder.Configuration["JWT:Audience"],
            // should lifetime be checked?
            ValidateLifetime = true,
            // 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            // should singinkey be checked
            ValidateIssuerSigningKey = true,
         };
});

var app = builder.Build();
 
app.UseAuthentication();
app.UseAuthorization();
 
app.Map("/login/{username}", (string username) => 
{
    var claims = new List<Claim> {new Claim(ClaimTypes.Name, username) };
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(app.Configuration["JWT:Key"]));
    // create jwt token
    var jwt = new JwtSecurityToken(
            issuer: app.Configuration["JWT:Issuer"],
            audience: app.Configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            
    return new JwtSecurityTokenHandler().WriteToken(jwt);
});
 
 
app.Run();
 