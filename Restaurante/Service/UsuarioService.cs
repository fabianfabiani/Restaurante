using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
        public UsuarioService(DataBaseContext context, IMapper mapper, AuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<List<UsuarioResponseDto>> GetAllUsuarios()
        {
            var usuarios = await _context.Usuarios
            .Include(e => e.Rol)     
            .Include(e => e.Sector)  
            .ToListAsync();

            
            return _mapper.Map<List<UsuarioResponseDto>>(usuarios);
        }

        public async Task<ActionResult<UsuarioResponseDto>> GetById(int idUsuario)
        {
            var usuario = await _context.Usuarios
            .Include(x => x.Rol)
            .Include (x => x.Sector)
            .FirstOrDefaultAsync(e => e.Id == idUsuario);

            if (usuario == null)
            {
                throw new Exception("Usuario inexistente");
            }

           
            var usuarioResponse = _mapper.Map<UsuarioResponseDto>(usuario);
            return usuarioResponse;
        }

        public async Task<ActionResult<UsuarioResponseDto>> Create(UsuarioRequestCreateDto usuario)
        {
            var rol = await _context.Roles.FindAsync(usuario.IdRol);
            var sector = await _context.Sectores.FindAsync(usuario.IdSector);

            // Verificar si se encontraron los roles y sectores
            if (rol == null || sector == null)
            {
                throw  new Exception("rol o sector no encontrado");
            }

            var nuevoUsuario = _mapper.Map<Usuario>(usuario);
            nuevoUsuario.Rol = rol; 
            nuevoUsuario.Sector = sector; 

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            
            var UsuarioResponse = _mapper.Map<UsuarioResponseDto>(nuevoUsuario);

            return UsuarioResponse;
        }

        public string LoguearUsuario(CredencialesDto credencialesDto)
        {
            UsuarioResponseDto usuario = this.ObtenerUsuarioPorCredenciales(credencialesDto);
            return this._authService.CreateToken(usuario);
        }

        private UsuarioResponseDto ObtenerUsuarioPorCredenciales(CredencialesDto credenciales)
        {
            // Cargar también las entidades relacionadas Rol y Sector
            Usuario? usuarioEncontrado = this._context.Usuarios
                .Include(u => u.Rol)      // Eager loading de la relación Rol
                .Include(u => u.Sector)   // Eager loading de la relación Sector
                .Where(u => u.Nombre == credenciales.User && u.Password == credenciales.Password)
                .FirstOrDefault();

            if (usuarioEncontrado == null)
            {
                throw new Exception("Usuario o contraseña incorrectos.");
            }

            return this._mapper.Map<UsuarioResponseDto>(usuarioEncontrado);
        }

    }
}
