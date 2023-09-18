using Microsoft.AspNetCore.Mvc;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Inmobiliaria23.Controllers
{
public class UsuariosController : Controller
{
        private readonly UsuarioRepositorio repositorioUsuario;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public UsuariosController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            repositorioUsuario = new UsuarioRepositorio();
            this.environment = environment;
            this.configuration = configuration;
        }

        
        // GET: Usuarios
       [Authorize(Policy = "Admin")]
        public ActionResult Index()
        {
             var usuario = repositorioUsuario.GetUsuarios();
            try
            {
                if (TempData.ContainsKey("Id"))
                    ViewBag.Mensaje = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
				    ViewBag.Mensaje = TempData["Mensaje"];
               
                return View(usuario);
            }
            catch (Exception)
            {

                return View();
            }

        }


        // GET: Usuarios/Details/5
       [Authorize(Policy = "Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Usuarios/Create
       [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
           ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       [Authorize(Policy = "Admin")]
        public ActionResult Create(Usuario u)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                u.Clave = hashed;
                u.Rol = User.IsInRole("Admin") ? u.Rol : (int)enRoles.Empleado;
                int res = repositorioUsuario.Alta(u);
                if (u.AvatarFile != null && u.Id > 0)
                {
                    string path = @"wwwroot\Uploads";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    
                    var nbreRnd = Guid.NewGuid();
                    string fileName = "avatar_" + u.Id + nbreRnd + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);                     
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(u);
                }
                if (res > 0)
                {
                    TempData["Mensaje"] = "Usuario creado correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        // GET: Usuarios/Edit/5
       [Authorize]
        public ActionResult Perfil()
        {
            // if (TempData.ContainsKey("Id"))
            //     ViewBag.Id = TempData["Id"];
            // if (TempData.ContainsKey("Mensaje"))
            //     ViewBag.Mensaje = TempData["Mensaje"];
            ViewData["Title"] = "Mi perfil";
            var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Perfil", u);
        }

        // GET: Usuarios/Edit/5
       [Authorize(Policy = "Admin")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repositorioUsuario.GetUsuario(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, Usuario user)
        {
            Usuario u = new Usuario();
            try
            {
                u = repositorioUsuario.GetUsuario(id);
                u.Nombre = user.Nombre;
                u.Apellido = user.Apellido;
                var cambiaMail = (user.Email != u.Email) ? true : false;
                u.Email = user.Email;
                u.Rol = user.Rol;
                if (repositorioUsuario.Modificacion(u) > 0 && cambiaMail)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                    await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
                }

                if (user.AvatarFile != null && u.Id > 0)
                {
                    var ruta = @"wwwroot" + u.Avatar;
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    var nbreRnd = Guid.NewGuid();
                    string path = @"wwwroot\Uploads";
                    string fileName = "avatar_" + u.Id + nbreRnd + Path.GetExtension(user.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = @"/Uploads/" + fileName;
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        user.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(u);
                }

                if (user.Clave != null && User.IsInRole("Admin"))
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: user.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                    u.Clave = hashed;
                    repositorioUsuario.Modificacion(u);
                }

                TempData["Mensaje"] = "Datos guardados correctamente";
               if (!User.IsInRole("Admin")) { return RedirectToAction(nameof(Perfil)); }
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CambiarClave(int id, String claveNueva, String clave, String confirmarNueva)
        {
            Usuario u = new Usuario();
            string hashed;
            try
            {
                if (clave == null)
                {
                    TempData["Mensaje"] = "La contraseña no puede estar vacía";
                    return RedirectToAction(nameof(Perfil));
                }

                u = repositorioUsuario.GetUsuario(id);
                if (claveNueva == confirmarNueva)
                {
                    hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                    password: claveNueva,
                                                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                                    prf: KeyDerivationPrf.HMACSHA1,
                                                    iterationCount: 1000,
                                                    numBytesRequested: 256 / 8));
                    claveNueva = hashed;
                }
                else
                {
                    TempData["Mensaje"] = "Las contraseñas no coinciden";
                    return RedirectToAction(nameof(Perfil));
                }

                hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                 password: clave,
                                                 salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                                 prf: KeyDerivationPrf.HMACSHA1,
                                                 iterationCount: 1000,
                                                 numBytesRequested: 256 / 8));
                clave = hashed;

                if (clave == u.Clave)
                {
                    u.Clave = claveNueva;
                    repositorioUsuario.Modificacion(u);
                    TempData["Mensaje"] = "Contraseña actualizada";
                }
                else
                {
                    TempData["Mensaje"] = "Contraseña actual invalida";
                    return RedirectToAction(nameof(Perfil));
                }

                return RedirectToAction(nameof(Perfil));
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // GET: Usuarios/Delete/5
       [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
            var u = repositorioUsuario.GetUsuario(id);
            return View(u);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       [Authorize(Policy = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var u = repositorioUsuario.GetUsuario(id);
                var ruta = @"wwwroot" + u.Avatar;

                if (repositorioUsuario.Baja(id) > 0)
                {
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }                   
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home/index" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "Email o clave incorrectos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                //TempData["returnUrl"] = returnUrl;
                return View("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Mensaje"] = "Hasta pronto";
            return RedirectToAction("Index", "Home");
        }

    }

}