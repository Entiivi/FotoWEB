import React from 'react';
import { useLocation } from 'react-router-dom';
import './css/PhotoDetails.css';

const PhotoDetails = () => {
    const location = useLocation();
    const { fotoData, pavadinimas, aprasymas } = location.state || {};

    return (
        <div className="photo-details-container">
            <img src={`data:image/jpeg;base64,${fotoData}`} alt={pavadinimas} />
            <h2>{pavadinimas}</h2>
            <p>{aprasymas}</p>
        </div>
    );
};

export default PhotoDetails;
