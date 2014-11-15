using System;

namespace Kangou.Core.Helpers
{
	public class StringMessages
	{
		public const string CONFIRM_MESSAGE_IOS = "\nUna vez confirmada la orden, buscaremos al Kangou más cercano para llevarte lo que deseas. \n\nEl cargo a la tarjeta se realizará hasta que se encuentre un Kangou para realizar tu pedido:\n\n${0}.00 MXN del envío ({1} Km) \n\nEn caso de compra, el cargo de ésta se realizará hasta que se compre lo ordenado más un 5% de comisión.";
		public const string ERROR_ORDER_RESPONSE_TITLE = "Error al procesar la orden";
		public const string ERROR_ORDER_RESPONSE_MESSAGE = "Compruebe su conexión a Internet e intente de nuevo.";
		public const string ORDER_CONFIRMED_MESSAGE = "¡Tu orden ha sido registrado exitosamente!\n\nEn unos breves minutos te enviaremos un correo de confirmación.\n\n¡Gracias!";
	}
}

