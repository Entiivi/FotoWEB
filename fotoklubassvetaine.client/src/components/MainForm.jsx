import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Box, Grid, Paper, Typography, CircularProgress } from "@mui/material";
import "./css/PhotoList.css";

function MainForm() {
    const [photos, setPhotos] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPhotos = async () => {
            try {
                const response = await fetch("https://localhost:7295/fotografija");
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
        <Box sx={{ display: "flex", minHeight: "100vh", backgroundColor: "#f5f5f5", padding: 2 }}>
            {/* Sidebar */}
            <Box
                sx={{
                    width: "20%",
                    padding: 3,
                    backgroundColor: "#e0e0e0",
                    display: "flex",
                    flexDirection: "column",
                    gap: 2,
                    boxShadow: 2,
                }}
            >
                <Button variant="contained" color="primary" onClick={() => navigate("/upload-photo")}>
                    Upload Photo
                </Button>
                <Button variant="contained" color="secondary" onClick={() => navigate("/login-info")}>
                    Login Information
                </Button>
                <Button variant="contained" color="success" onClick={() => navigate("/personal-info")}>
                    Personal Information
                </Button>
            </Box>

            {/* Photo Gallery */}
            <Box sx={{ flexGrow: 1, padding: 4 }}>
                {loading ? (
                    <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100%" }}>
                        <CircularProgress />
                    </Box>
                ) : photos.length === 0 ? (
                    <Typography variant="h6" align="center" color="textSecondary">
                        No photos available.
                    </Typography>
                ) : (
                    <Grid container spacing={2}>
                        {photos.map((photo) => (
                            <Grid item xs={12} sm={6} md={4} key={photo.fotoID}>
                                <Paper
                                    elevation={3}
                                    sx={{
                                        overflow: "hidden",
                                        borderRadius: "8px",
                                        cursor: "pointer",
                                        "&:hover img": { transform: "scale(1.05)", transition: "0.3s" },
                                    }}
                                    onClick={() => handlePhotoClick(photo)}
                                >
                                    <img
                                        src={`https://localhost:7295/${photo.fotoPath}`}
                                        alt={photo.pavadinimas}
                                        style={{ width: "100%", height: "200px", objectFit: "cover" }}
                                    />
                                </Paper>
                            </Grid>
                        ))}
                    </Grid>
                )}
            </Box>
        </Box>
    );
}

export default MainForm;
