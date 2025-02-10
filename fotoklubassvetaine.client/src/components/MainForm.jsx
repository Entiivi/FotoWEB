import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/PhotoList.css';

function MainForm() {
    const [photos, setPhotos] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPhotos = async () => {
            try {
                const response = await fetch('https://localhost:7295/fotografija');
                if (response.ok) {
                    const data = await response.json();
                    setPhotos(data);
                } else {
                    console.error('Failed to fetch photos');
                }
            } catch (error) {
                console.error('Error fetching photos:', error);
            }
        };

        fetchPhotos();
    }, []);

    const handlePhotoClick = (photo) => {
        navigate('/photo-details', { state: photo });
    };

    return (
        <div className="photo-list-container">
            <div className="sidebar">
                <button onClick={() => navigate('/upload-photo')}>Upload Photo</button>
                <button onClick={() => navigate('/login-info')}>Login Information</button>
                <button onClick={() => navigate('/personal-info')}>Personal Information</button>
            </div>
            <div className="photo-list">
                {photos.map(photo => (
                    <img
                        key={photo.fotoID}
                        src={`https://localhost:7295/${photo.fotoPath}`}
                        alt={photo.pavadinimas}
                        onClick={() => handlePhotoClick(photo)}
                    />
                ))}
            </div>
        </div>
    );
}

export default MainForm;
