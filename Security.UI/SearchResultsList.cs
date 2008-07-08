/*
AdamMil.Security.UI is a .NET library providing common user interface widgets
for OpenPGP-based software.
Copyright (C) 2008 Adam Milazzo (http://www.adammil.net/)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AdamMil.Security.PGP;

namespace AdamMil.Security.UI
{

public class SearchResultsList : PGPListBase
{
  public SearchResultsList()
  {
    InitializeControl();
  }

  public void AddResults(PrimaryKey[] publicKeys)
  {
    if(publicKeys == null) throw new ArgumentNullException();

    foreach(PrimaryKey key in publicKeys)
    {
      if(key == null) throw new ArgumentException("A key was null.");

      ListViewItem item = CreateResultItem(key);
      if(item != null)
      {
        item.Tag = key;
        Items.Add(item);
      }
    }
  }

  public PrimaryKey[] GetSelectedKeys()
  {
    PrimaryKey[] keys = new PrimaryKey[CheckedItems.Count];
    for(int i=0; i<keys.Length; i++) keys[i] = (PrimaryKey)CheckedItems[i].Tag;
    return keys;
  }

  public string[] GetSelectedIds()
  {
    string[] keys = new string[CheckedItems.Count];
    for(int i=0; i<keys.Length; i++) keys[i] = ((PrimaryKey)CheckedItems[i].Tag).EffectiveId;
    return keys;
  }

  protected virtual ListViewItem CreateResultItem(PrimaryKey key)
  {
    ListViewItem item = new ListViewItem(PGPUI.GetKeyName(key));
    item.SubItems.Add(key.CreationTime.ToShortDateString());
    item.SubItems.Add(key.ShortKeyId);
    item.SubItems.Add(key.Revoked ? "Revoked" : key.Expired ? "Expired" : "Valid");
    if(key.Revoked || key.Expired) item.ForeColor = SystemColors.GrayText;
    return item;
  }

  void InitializeControl()
  {
    base.CheckBoxes = true;

    ColumnHeader userIdHeader, createdHeader, keyIdHeader, statusHeader;

    userIdHeader = new ColumnHeader();
    userIdHeader.Text = "User ID";
    userIdHeader.Width = 400;

    createdHeader = new ColumnHeader();
    createdHeader.Text = "Created";
    createdHeader.Width = 75;

    keyIdHeader = new ColumnHeader();
    keyIdHeader.Text = "Key ID";
    keyIdHeader.Width = 75;

    statusHeader = new ColumnHeader();
    statusHeader.Text = "Status";
    statusHeader.Width = 90;

    Columns.AddRange(new ColumnHeader[] { userIdHeader, createdHeader, keyIdHeader, statusHeader });
  }
}

} // namespace AdamMil.Security.UI