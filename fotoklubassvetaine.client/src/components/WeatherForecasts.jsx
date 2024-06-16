import { useEffect, useState } from 'react';
import { getWeatherForecasts } from '../services/api';

const WeatherForecasts = () => {
    const [forecasts, setForecasts] = useState([]);

    useEffect(() => {
        fetchForecasts();
    }, []);

    const fetchForecasts = async () => {
        try {
            const data = await getWeatherForecasts();
            setForecasts(data);
        } catch (error) {
            console.error('Error fetching forecasts', error);
        }
    };

    return (
        <div>
            <h1>Weather Forecasts</h1>
            <ul>
                {forecasts.map((forecast, index) => (
                    <li key={index}>
                        {forecast.date}: {forecast.temperatureC}°C ({forecast.summary})
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default WeatherForecasts;
