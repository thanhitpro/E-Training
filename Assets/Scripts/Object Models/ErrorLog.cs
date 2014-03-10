using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETraining
{
	public class ErrorLog {
		/// <summary>
		/// The identifier log.
		/// </summary>
		int idLog;

		public int IdLog {
			get {
				return idLog;
			}
			set {
				idLog = value;
			}
		}

		/// <summary>
		/// The type log.
		/// We have 3 type of log: 
		/// 1. Critical Error log
		/// 2. Normal Error log
		/// 3. Warning log
		/// </summary>
		int typeLog;

		public int TypeLog {
			get {
				return typeLog;
			}
			set {
				typeLog = value;
			}
		}

		/// <summary>
		/// The content log.
		/// </summary>
		string contentLog;

		public string ContentLog {
			get {
				return contentLog;
			}
			set {
				contentLog = value;
			}
		}


		public ErrorLog() {
			idLog = -1;
			typeLog = 0;
			contentLog = "";
		}

		public ErrorLog(int id) {
			idLog = id;
			typeLog = 0;
			contentLog = "";
		}

		public ErrorLog(int id, int type, string content) {
			idLog = id;
			typeLog = type;
			contentLog = content;
		}
	}
}