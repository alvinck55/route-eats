import { useState } from 'react'
import { useCategories } from '../hooks/useCategories'
import { CategoryFilter } from './CategoryFilter'

export function SearchForm({ onSearch, loading }) {
  const [origin, setOrigin] = useState('')
  const [destination, setDestination] = useState('')
  const [selectedCategories, setSelectedCategories] = useState([])

  const { categories } = useCategories()

  const handleSubmit = (e) => {
    e.preventDefault()
    if (!origin || !destination || selectedCategories.length === 0) return
    onSearch(origin, destination, selectedCategories)
  }

  return (
    <form className="search-form" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Origin (e.g. Austin, TX)"
        value={origin}
        onChange={(e) => setOrigin(e.target.value)}
        required
      />
      <input
        type="text"
        placeholder="Destination (e.g. Houston, TX)"
        value={destination}
        onChange={(e) => setDestination(e.target.value)}
        required
      />

      <CategoryFilter
        categories={categories}
        selected={selectedCategories}
        onChange={setSelectedCategories}
      />

      <button type="submit" disabled={loading || selectedCategories.length === 0}>
        {loading ? 'Searching…' : 'Find Food Along Route'}
      </button>
    </form>
  )
}
