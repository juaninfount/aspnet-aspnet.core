import React from 'react'
import { Box, Button, TextField } from '@mui/material';


export default function Login(){
    return (
        <Box sx={{'& .MuiTextField-root':{
                m:1
            }
        }} >
            <form noValidate autoComplete='off'>
                <TextField  label="Email" name="Email" variant='outlined'>
                </TextField>
                <TextField  label="Name" name="Name" variant='outlined'>
                </TextField>
                <Button  type='submit'>Start</Button> 
            </form>
        </Box>  
    )
}