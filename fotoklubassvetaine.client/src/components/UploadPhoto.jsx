import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './css/UploadPhoto.css';

const fetchAntiForgeryToken = async () => {
    try {
        const response = await fetch('https://localhost:7295/antiforgery-token', {
            credentials: 'include', // Ensures cookies are included
        });
        const data = await response.json();
        return data.token; // Return the anti-forgery token
    } catch (error) {
        console.error('Failed to fetch anti-forgery token:', error);
        throw error;
    }
};


const UploadPhoto = ({ username }) => {
    const [photo, setPhoto] = useState(null);
    const [photoPreview, setPhotoPreview] = useState(null);
    const [pavadinimas, setPavadinimas] = useState('');
    const [aprasymas, setAprasymas] = useState('');
    const [narysID, setNarysID] = useState(null);
    const [klubasID, setKlubasID] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    useEffect(() => {
        const fetchUserInfo = async () => {
            try {
                const response = await fetch(`https://localhost:7295/user/userinfo?username=${username}`);
                if (response.ok) {
                    const data = await response.json();
                    setNarysID(data.narysID);
                    setKlubasID(data.klubasID);
                } else {
                    setError('Failed to fetch user information.');
                }
            } catch (err) {
                setError('Error fetching user information.');
                console.error(err);
            }
        };
        fetchUserInfo();
    }, [username]);

    const handlePhotoChange = (e) => {
        const file = e.target.files[0];
        if (!file) {
            setError('No file selected.');
            return;
        }

        if (file.type !== 'image/jpeg') {
            setError('Only .jpg files are allowed.');
            return;
        }

        const minSize = 5 * 1024 * 1024; // 30MB
        const maxSize = 50 * 1024 * 1024; // 50MB

        if (file.size < minSize) {
            setError('File size must be at least 30MB.');
            return;
        }

        if (file.size > maxSize) {
            setError('File size must be less than 50MB.');
            return;
        }

        setPhoto(file);
        setError('');

        const reader = new FileReader();
        reader.onloadend = () => {
            setPhotoPreview(reader.result);
        };
        reader.onerror = () => {
            console.error('Error reading file:', reader.error);
            setError('Error generating photo preview.');
        };
        reader.readAsDataURL(file);
    };

    const handleUpload = async () => {
        const token = await fetchAntiForgeryToken();
        if (!pavadinimas || !aprasymas || !photo) {
            setError('All fields are required.');
            return;
        }

        setLoading(true);
        setError('');
        setSuccess('');

        const formData = new FormData();
        formData.append('photo', photo);
        formData.append('pavadinimas', pavadinimas);
        formData.append('aprasymas', aprasymas);
        formData.append('narysID', narysID);
        formData.append('klubasID', klubasID);

        try {
            const response = await fetch('https://localhost:7295/upload', {
                method: 'POST',
                headers: {
                    'X-CSRF-TOKEN': token, // Include the anti-forgery token
                },
                body: formData,
                credentials: 'include',
            });

            if (response.ok) {
                const data = await response.json();
                setSuccess(data.Message);
                setPhoto(null);
                setPhotoPreview(null);
                setPavadinimas('');
                setAprasymas('');
                console.log('Upload successful:', data);
            } else {
                const errorText = await response.text();
                setError(errorText || 'Failed to upload photo.');
                console.error('Upload failed:', errorText);
            }
        } catch (error) {
            console.error('Upload error:', error);
            setError('An error occurred during upload.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="upload-photo-container">
            <h2>Upload Photo</h2>
            <div className="upload-warning">
                <p>Please upload a .jpg format photo</p>
            </div>

            {error && <div className="error-message">{error}</div>}
            {success && <div className="success-message">{success}</div>}

            <div className="form-container">
                <div className="left-column">
                    <div className="input-container">
                        <label htmlFor="pavadinimas">Name:</label>
                        <input
                            type="text"
                            id="pavadinimas"
                            value={pavadinimas}
                            onChange={(e) => setPavadinimas(e.target.value)}
                        />
                    </div>
                    <div className="input-container">
                        <label htmlFor="aprasymas">About:</label>
                        <textarea
                            id="aprasymas"
                            value={aprasymas}
                            rows="4"
                            onChange={(e) => setAprasymas(e.target.value)}
                        ></textarea>
                    </div>
                </div>

                <div className="right-column">
                    <div className="input-container">
                        <label htmlFor="photo">Photo:</label>
                        <input
                            type="file"
                            id="photo"
                            accept="image/jpeg"
                            onChange={handlePhotoChange}
                        />
                    </div>
                    <div className="button-container">
                        <button onClick={handleUpload} disabled={loading}>
                            {loading ? 'Uploading...' : 'Upload'}
                        </button>
                    </div>
                    <div className="photo-preview">
                        {photoPreview ? <img src={photoPreview} alt="Preview" /> : <span>.JPG</span>}
                    </div>
                </div>
            </div>
        </div>
    );
};

UploadPhoto.propTypes = {
    username: PropTypes.string.isRequired,
};

export default UploadPhoto;
