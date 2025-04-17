import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Typography, CircularProgress } from "@mui/material";
import "./css/MainForm.css";
import "./css/PhotoList.css";

function MainForm() {
    const [photos, setPhotos] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPhotos = async () => {
            try {
                const response = await fetch("https://localhost:7001/fotografija");
                if (response.ok) {
                    const data = await response.json();
                    setPhotos(data);
                } else {
                    console.error("Failed to fetch photos");
                }
            } catch (error) {
                console.error("Error fetching photos:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchPhotos();
    }, []);

    const handlePhotoClick = (photo) => {
        navigate("/photo-details", { state: photo });
    };

    return (
        <div className="main-form-wrapper">
            {/* Sidebar */}
            <div className="sidebar1">
                <Button className="side-button"
                    variant="contained"
                    color="primary"
                    onClick={() => navigate("/upload-photo")}
                >
                    Upload Photo
                </Button>
                <Button className="side-button"
                    variant="contained"
                    color="secondary"
                    onClick={() => navigate("/login-info")}
                >
                    Login Information
                </Button>
                <Button className="side-button"
                    variant="contained"
                    color="success"
                    onClick={() => navigate("/personal-info")}
                >
                    Personal Information
                </Button>
            </div>

            {/* Photo Gallery */}
            <section className="photo-gallery">
                {loading ? (
                    <div className="loading-container">
                        <CircularProgress />
                    </div>
                ) : photos.length === 0 ? (
                    <Typography variant="h6" className="no-photos-text">
                        No photos available.
                    </Typography>
                ) : (
                    <div className="grid-container">
                        {photos.map((photo) => (
                            <div
                                key={photo.fotoID}
                                className="photo-card"
                                onClick={() => handlePhotoClick(photo)}
                            >
                                <img
                                    src={`https://localhost:7001/${photo.fotoPath}`}
                                    alt={photo.pavadinimas}
                                    className="photo-image"
                                />
                            </div>
                        ))}
                    </div>
                )}
            </section>
        </div>
    );
}

export default MainForm;
