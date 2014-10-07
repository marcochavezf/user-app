using Android.App;
using Android.Content;

namespace Kangou.Droid.Views
{
	public class BindableProgress
	{
		private readonly Context _context;

		public BindableProgress(Context context)
		{
			_context = context;
		}

		private ProgressDialog _dialog;

		public bool Visible
		{
			get { return _dialog != null; }
			set
			{
				if (value == Visible)
					return;

				if (value)
				{
					_dialog = ProgressDialog.Show (_context, "Guardando datos", "Espere por favor...", true);
				}
				else
				{
					_dialog.Dismiss ();
					_dialog = null;
				}
			}
		}
	}
}
