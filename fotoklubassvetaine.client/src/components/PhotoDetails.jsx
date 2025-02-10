import React from 'react';
import { useLocation } from 'react-router-dom';
import './css/PhotoDetails.css';

const PhotoDetails = () => {
    const location = useLocation();
    const { fotoPath, pavadinimas, aprasymas } = location.state || {};

    return (
        <div className="photo-details-container">
            <img src={`https://localhost:7295/${fotoPath}`} alt={pavadinimas} />
            <h2>{pavadinimas}</h2>
            <p>{aprasymas}</p>
        </div>
    );
};

export default PhotoDetails;
