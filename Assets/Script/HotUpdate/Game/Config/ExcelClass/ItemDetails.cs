using System.Collections.Generic;
using MyFrameworkCore;
using System;
	
[Serializable]
public class ItemDetails : IData
{
	public int       	itemID;
	public string    	name;
	public int       	itemType;
	public string    	itemIconName;
	public string    	itemIconOnWorldName;
	public string    	itemDescription;
	public int       	itemUseRadiue;
	public bool      	canPickedup;
	public bool      	canDropped;
	public bool      	canCarried;
	public int       	itemPrice;
	public float     	sellPercentage;
    public int GetId()
    {
		return itemID;
    }
}
