using System;

namespace Kangou.Core
{
	public class SocketEvents
	{
		public const string Connected 				= "connected";
		public const string ActiveOrder 			= "activeOrder";
		public const string KangouGoingToPickUp 	= "kangouGoingToPickUp";
		public const string KangouWaitingToPickUp 	= "kangouWaitingToPickUp";
		public const string KangouGoingToDropOff 	= "kangouGoingToDropOff";
		public const string KangouWaitingToDropOff 	= "kangouWaitingToDropOff";
		public const string OrderSignedByClient 	= "orderSignedByClient";
		public const string OrderReviewedByClient 	= "orderReviewedByClient";
		public const string ReviewAcceptedByClient 	= "reviewAcceptedByClient";
		public const string KangouPosition		 	= "kangouPosition";
	}
}

