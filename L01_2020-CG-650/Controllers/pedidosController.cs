using L01_2020_CG_650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_CG_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;

        public pedidosController(restauranteContext restauranteContexto)
        { 
            _restauranteContext = restauranteContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<pedidos> listadoPedidos = (from e in _restauranteContext.pedidos
                                           select e).ToList();

            if (listadoPedidos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPedidos);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] pedidos pedido)
        {
            try
            {
                _restauranteContext.pedidos.Add(pedido);
                _restauranteContext.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPedido(int id, [FromBody] pedidos pedidoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            pedidos? pedidoActual = (from e in _restauranteContext.pedidos
                                     where e.pedidoId == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (pedidoActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            pedidoActual.motoristaId = pedidoModificar.motoristaId;
            pedidoActual.clienteId = pedidoModificar.clienteId;
            pedidoActual.platoId = pedidoModificar.platoId;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos
            _restauranteContext.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(pedidoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPedido(int id)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual eliminaremos
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (pedido == null)
                return NotFound();

            //Ejecutamos la accion de eliminar el registro
            _restauranteContext.pedidos.Attach(pedido);
            _restauranteContext.pedidos.Remove(pedido);
            _restauranteContext.SaveChanges();

            return Ok(pedido);
        }

        /// <summary>
        /// EndPoint que retorna los registros de una tabla filtrados por el clienteId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByIdCliente/{id}")]
        public IActionResult GetCliente(int id)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.clienteId == id
                               select e).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        /// <summary>
        /// EndPoint que retorna los registros de una tabla filtrados por el motorista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByIdMotorista/{id}")]
        public IActionResult GetMotorista(int id)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.motoristaId == id
                               select e).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }
    }
}
