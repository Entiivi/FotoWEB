import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './css/UploadPhoto.css';

const UploadPhoto = ({ username }) => {
    const [photo, setPhoto] = useState(null);
    const [photoPreview, setPhotoPreview] = useState(null);
    const [pavadinimas, setPavadinimas] = useState('');
    const [aprasymas, setAprasymas] = useState('');
    const [narysID, setNarysID] = useState(null);
    const [klubasID, setKlubasID] = useState(null);

    const fetchUserInfo = async () => {
        try {
            const response = await fetch(`https://localhost:7295/user/userinfo?username=${username}`);
            if (response.ok) {
                const data = await response.json();
                setNarysID(data.narysID);
                setKlubasID(data.klubasID);
            } else {
                console.error('Failed to fetch user info');
            }
        } catch (error) {
            console.error('Error fetching user info:', error);
        }
    };

    useEffect(() => {
        fetchUserInfo();
    }, [username]);

    const handlePhotoChange = (e) => {
        const file = e.target.files[0];
        setPhoto(file);

        const reader = new FileReader();
        reader.onloadend = () => {
            setPhotoPreview(reader.result);
        };
        reader.readAsDataURL(file);
    };

    const handleUpload = async () => {
        const formData = new FormData();
        formData.append('photo', photo);
        formData.append('pavadinimas', pavadinimas);
        formData.append('aprasymas', aprasymas);
        formData.append('narysID', narysID);  // Include NarysID
        formData.append('klubasID', klubasID); // Include KlubasID

        try {
            const response = await fetch('https://localhost:7295/upload', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                alert('Photo uploaded successfully');
            } else {
                alert('Failed to upload photo');
            }
        } catch (error) {
            console.error('Error uploading photo:', error);
        }
    };

    return (
        <div className="upload-photo-container">
            <h2>Upload Photo</h2>
            <div className="upload-warning">
                <p>Please upload a .jpg format photo</p>
            </div>
            <div className="input-container">
                <label htmlFor="pavadinimas">Pavadinimas:</label>
                <input
                    type="text"
                    id="pavadinimas"
                    value={pavadinimas}
                    onChange={(e) => setPavadinimas(e.target.value)}
                />
            </div>
            <div className="input-container">
                <label htmlFor="aprasymas">Aprasymas:</label>
                <input
                    type="text"
                    id="aprasymas"
                    value={aprasymas}
                    onChange={(e) => setAprasymas(e.target.value)}
                />
            </div>
            <div className="input-container">
                <label htmlFor="photo">Photo:</label>
                <input
                    type="file"
                    id="photo"
                    accept=".jpg"
                    onChange={handlePhotoChange}
                />
            </div>
            {photoPreview && (
                <div className="photo-preview">
                    <h3>Photo Preview:</h3>
                    <img
                        src={photoPreview}
                        alt="Preview"
                    />
                </div>
            )}
            <button onClick={handleUpload}>Upload</button>
        </div>
    );
};

UploadPhoto.propTypes = {
    username: PropTypes.string.isRequired,
};

export default UploadPhoto;
