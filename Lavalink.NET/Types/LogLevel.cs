using System;
using System.Collections.Generic;
using System.Text;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Different levels of logs.
	/// </summary>
    public enum LogLevel
	{
		/// <summary>
		/// Shows from Debug to Error all logs. usefull in developement.
		/// </summary>
		Debug,
		/// <summary>
		/// Shows from Info to Error all logs. usefull in production.
		/// </summary>
		Info,
		/// <summary>
		/// Shows warnings and Errors. usefull in production when you dont want that much output.
		/// </summary>
		Warning,
		/// <summary>
		/// Only shows errors. usefull if you only want critical information.
		/// </summary>
		Error
	}
}
