import { useState } from 'react'
import { SearchForm } from './components/SearchForm'
import { MapView } from './components/MapView'
import { RestaurantCard } from './components/RestaurantCard'
import { getRouteSuggestions } from './services/api'
import './App.css'

export default function App() {
  const [restaurants, setRestaurants] = useState([])
  const [selectedId, setSelectedId] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const handleSearch = async (origin, destination, categories) => {
    setLoading(true)
    setError(null)
    setRestaurants([])
    setSelectedId(null)

    try {
      const results = await getRouteSuggestions(origin, destination, categories)
      setRestaurants(results)
    } catch (err) {
      setError(err.response?.data ?? 'Something went wrong. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="app">
      <aside className="sidebar">
        <header className="sidebar__header">
          <h1>RouteEats</h1>
          <p>Discover food along your drive</p>
        </header>

        <SearchForm onSearch={handleSearch} loading={loading} />

        {error && <p className="error">{error}</p>}

        <div className="results">
          {restaurants.map((r) => (
            <RestaurantCard
              key={r.placeId}
              restaurant={r}
              isSelected={r.placeId === selectedId}
              onSelect={(r) => setSelectedId(r.placeId)}
            />
          ))}
        </div>
      </aside>

      <main className="map-container">
        <MapView
          restaurants={restaurants}
          selectedId={selectedId}
          directions={null}
          onMarkerClick={(r) => setSelectedId(r.placeId)}
        />
      </main>
    </div>
  )
}
