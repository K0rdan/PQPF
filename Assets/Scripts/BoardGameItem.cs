using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NSBoard;
using NSBoardSquare;

namespace NSBoardGameItem
{
	public interface ILoad
	{
		bool							loadFromFile (string fileName);
	}

	public interface ISave
	{
		bool	 						save (string fileName);
	}

	public abstract class BoardGameItem : MonoBehaviour, ILoad, ISave
	{
		//Property[] 						_properties;
		protected Dictionary<string, Property> 	_properties;

		public abstract bool			loadFromFile (string fileName);

		public abstract bool	 		save (string fileName);
	
		public abstract void Start ();

		public abstract void Update ();
	}

	public abstract class Property : BoardGameItem
	{
		public string					_name;
		public int 						_attribute;
		//public
		public abstract void 			action ();

		public abstract bool			condition (ref Board board, ref BoardSquare bsquare, ref Card card);
	}

}