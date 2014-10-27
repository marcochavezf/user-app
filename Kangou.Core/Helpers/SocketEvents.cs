using System;

namespace Kangou.Core
{
	public class SocketEvents
	{
		public const string Connected 				= "connected";
		public const string ActiveOrder 			= "active_order";
		public const string KangouGoingToPickUp 	= "kangou_going_to_pick_up";
		public const string KangouWaitingToPickUp 	= "kangou_waiting_to_pick_up";
		public const string KangouGoingToDropOff 	= "kangou_going_to_drop_off";
		public const string KangouWaitingToDropOff 	= "kangou_waiting_to_drop_off";
		public const string OrderSignedByClient 	= "order_signed_by_client";
		public const string OrderReviewedByClient 	= "order_reviewed_by_client";
		public const string ReviewAcceptedByClient 	= "review_accepted_by_client";
		public const string KangouPosition		 	= "kangou_position";
	}
}

