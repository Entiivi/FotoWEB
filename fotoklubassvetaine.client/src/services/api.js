import axios from 'axios';

const API_URL = 'https://localhost:7295'; // Ensure this matches your backend URL

export const getWeatherForecasts = async () => {
    try {
        const response = await axios.get(`${API_URL}/weatherforecast`);
        return response.data;
    } catch (error) {
        console.error('Error fetching weather forecasts', error);
        throw error;
    }
};
