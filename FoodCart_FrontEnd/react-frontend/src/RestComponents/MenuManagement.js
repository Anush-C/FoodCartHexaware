import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Button, TextField, Dialog } from '@mui/material';

const MenuManagement = () => {
  const [menuItems, setMenuItems] = useState([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [menuItem, setMenuItem] = useState({ name: '', price: '', categoryID: '' });

  useEffect(() => {
    const fetchMenuItems = async () => {
      const response = await axios.get('/api/restaurant/menuitems/1'); // Replace 1 with the actual restaurant ID
      setMenuItems(response.data);
    };
    fetchMenuItems();
  }, []);

  const handleAddMenuItem = async () => {
    await axios.post('/api/restaurant/menuitem', menuItem);
    setOpenDialog(false);
    // Refresh menu items
  };

  return (
    <div>
      <Button variant="contained" onClick={() => setOpenDialog(true)}>Add Menu Item</Button>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Price</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {menuItems.map(item => (
              <TableRow key={item.id}>
                <TableCell>{item.name}</TableCell>
                <TableCell>${item.price}</TableCell>
                <TableCell>
                  <Button onClick={() => {/* Handle Update */}}>Edit</Button>
                  <Button onClick={() => {/* Handle Delete */}}>Delete</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <div>
          <TextField label="Name" value={menuItem.name} onChange={e => setMenuItem({ ...menuItem, name: e.target.value })} />
          <TextField label="Price" value={menuItem.price} onChange={e => setMenuItem({ ...menuItem, price: e.target.value })} />
          <Button onClick={handleAddMenuItem}>Add Item</Button>
        </div>
      </Dialog>
    </div>
  );
};

export default MenuManagement;
