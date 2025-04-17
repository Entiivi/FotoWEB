import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './css/PhotoDetails.css';

const PhotoDetails = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { fotoPath, pavadinimas, aprasymas } = location.state || {};

    return (
        <div className="photo-details-container">
            <button className="back-button" onClick={() => navigate(-1)}>
                Back
            </button>

            <div className="photo-card2">
                <img
                    src={`https://localhost:7001/${fotoPath}`}
                    alt={pavadinimas}
                    className="detail-image"
                />

                <h2 className="detail-title">{pavadinimas}</h2>
                <p className="detail-desc">{aprasymas}</p>
            </div>
        </div>
    );
};

export default PhotoDetails;
