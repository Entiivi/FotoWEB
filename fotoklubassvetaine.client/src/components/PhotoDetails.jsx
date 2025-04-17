import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './css/PhotoDetails.css';

const PhotoDetails = () => {
    const location = useLocation();
    const navigate = useNavigate();

    // Destructure state; make sure your MapPhoto click passed fotoID
    const { fotoID, fotoPath, pavadinimas, aprasymas } = location.state || {};

    const handleDelete = async () => {
        if (!fotoID) return;

        if (!window.confirm('Are you sure you want to delete this photo?')) return;

        try {
            const response = await fetch(
                `https://localhost:7001/fotografija/${fotoID}`,
                { method: 'DELETE' }
            );
            if (response.ok) {
                // go back to gallery once deleted
                navigate('/main');
            } else {
                alert('Failed to delete photo');
            }
        } catch (err) {
            console.error('Delete error:', err);
            alert('An error occurred');
        }
    };

    return (
        <div className="photo-details-container">
            <div className="photo-card2">
                <div className="photo-actions">
                    <button
                        className="back-button"
                        onClick={() => navigate(-1)}
                    >
                        Back
                    </button>
                    <button
                        className="delete-button"
                        onClick={handleDelete}
                    >
                        Delete
                    </button>
                </div>

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
