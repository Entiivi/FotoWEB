import React, { useEffect, useState } from 'react';
import axios from 'axios';

const PhotoList = () => {
    const [photos, setPhotos] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        axios.get('/photos')
            .then(response => {
                console.log('Full response:', response);
                const data = response.data.photos; // Adjust according to the actual structure
                if (Array.isArray(data)) {
                    setPhotos(data);
                } else {
                    setError('Unexpected response format');
                }
            })
            .catch(error => {
                console.error('There was an error fetching the photos!', error);
                setError('Error fetching photos');
            });
    }, []);

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h1>Photo List</h1>
            <ul>
                {photos.map(photo => (
                    <li key={photo.id}>
                        <h2>{photo.title}</h2>
                        <p>{photo.description}</p>
                        <img src={photo.imageUrl} alt={photo.title} />
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default PhotoList;
