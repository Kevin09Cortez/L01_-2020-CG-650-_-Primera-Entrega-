using L01_2020_CG_650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_CG_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristasController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;

        public motoristasController(restauranteContext restauranteContexto)
        {
            _restauranteContext = restauranteContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los motoristas existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<motoristas> listadoMotoristas = (from e in _restauranteContext.motoristas
                                          select e).ToList();

            if (listadoMotoristas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMotoristas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMotorista([FromBody] motoristas motorista)
        {
            try
            {
                _restauranteContext.motoristas.Add(motorista);
                _restauranteContext.SaveChanges();
                return Ok(motorista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] motoristas motoristaModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            motoristas? motoristaActual = (from e in _restauranteContext.motoristas
                                   where e.motoristaId == id
                                   select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (motoristaActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            motoristaActual.nombreMotorista = motoristaModificar.nombreMotorista;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _restauranteContext.Entry(motoristaActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(motoristaModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarMotorista(int id)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual eliminaremos
            motoristas? motorista = (from e in _restauranteContext.motoristas
                             where e.motoristaId == id
                             select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (motorista == null)
                return NotFound();

            //Ejecutamos la accion de eliminar el registro
            _restauranteContext.motoristas.Attach(motorista);
            _restauranteContext.motoristas.Remove(motorista);
            _restauranteContext.SaveChanges();

            return Ok(motorista);
        }

        ///<summary>
        ///EndPoint que retorna los registros de una tabla filtrados por el nombre del motorista
        ///</summary>
        ///<param name="id"></param>
        ///<returns></returns>
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByNombreMotorista(string filtro)
        {
            List<motoristas> listadoMotoristas = (from e in _restauranteContext.motoristas
                                                  where e.nombreMotorista.Contains(filtro)
                                                  select e).ToList();

            if (listadoMotoristas == null)
            {
                return NotFound();
            }

            return Ok(listadoMotoristas);
        }
    }
}
