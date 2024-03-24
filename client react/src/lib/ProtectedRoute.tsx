import React from 'react';
import { Outlet, Navigate } from "react-router-dom";

const ProtectedRoute = ():any => {
    const role :string= "ADMIN"
    return(
        role == "ADMIN" ? <Outlet /> : <Navigate to="/" />
    )

};

export default ProtectedRoute;