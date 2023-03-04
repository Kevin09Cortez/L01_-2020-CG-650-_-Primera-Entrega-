using L01_2020_CG_650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_CG_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;

        public platosController(restauranteContext restauranteContexto)
        {
            _restauranteContext = restauranteContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los platos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<platos> listadoPlatos = (from e in _restauranteContext.platos
                                            select e).ToList();

            if (listadoPlatos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPlatos);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPlato([FromBody] platos plato)
        {
            try
            {
                _restauranteContext.platos.Add(plato);
                _restauranteContext.SaveChanges();
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlato(int id, [FromBody] platos platoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            platos? platoActual = (from e in _restauranteContext.platos
                                     where e.platoId == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (platoActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            platoActual.nombrePlato = platoModificar.nombrePlato;
            platoActual.precio = platoModificar.precio;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _restauranteContext.Entry(platoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(platoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual eliminaremos
            platos? plato = (from e in _restauranteContext.platos
                               where e.platoId == id
                               select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (plato == null)
                return NotFound();

            //Ejecutamos la accion de eliminar el registro
            _restauranteContext.platos.Attach(plato);
            _restauranteContext.platos.Remove(plato);
            _restauranteContext.SaveChanges();

            return Ok(plato);
        }

        /// <summary>
        /// EndPoint que retorna los registros de una tabla filtrados por el precio
        /// </summary>
        /// <param name="precio"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByPrecio/{precio}")]
        public IActionResult GetPrecio(decimal precio)
        {
            List<platos> listadoPlatos = (from e in _restauranteContext.platos
                                          where e.precio < precio
                                          select e).ToList();

            if (listadoPlatos == null)
            {
                return NotFound();
            }
            return Ok(listadoPlatos);
        }
    }
}
