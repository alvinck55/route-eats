import { GoogleMap, useJsApiLoader, Marker, DirectionsRenderer } from '@react-google-maps/api'

const MAP_LIBRARIES = ['places']

export function MapView({ restaurants, directions, selectedId, onMarkerClick }) {
  const { isLoaded } = useJsApiLoader({
    googleMapsApiKey: import.meta.env.VITE_GOOGLE_MAPS_API_KEY ?? '',
    libraries: MAP_LIBRARIES,
  })

  if (!isLoaded) return <div className="map-loading">Loading map…</div>

  return (
    <GoogleMap
      mapContainerStyle={{ width: '100%', height: '100%' }}
      zoom={7}
      center={{ lat: 30.25, lng: -97.75 }} // Default: Austin, TX
    >
      {directions && <DirectionsRenderer directions={directions} />}

      {restaurants.map((r) => (
        <Marker
          key={r.placeId}
          position={{ lat: r.latitude, lng: r.longitude }}
          title={r.name}
          onClick={() => onMarkerClick(r)}
          icon={
            r.placeId === selectedId
              ? 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'
              : undefined
          }
        />
      ))}
    </GoogleMap>
  )
}
