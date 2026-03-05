export function RestaurantCard({ restaurant, onSelect, isSelected }) {
  return (
    <div
      className={`restaurant-card ${isSelected ? 'restaurant-card--selected' : ''}`}
      onClick={() => onSelect(restaurant)}
    >
      <h3>{restaurant.name}</h3>
      {restaurant.address && <p className="address">{restaurant.address}</p>}
      <div className="meta">
        {restaurant.rating != null && (
          <span className="rating">★ {restaurant.rating.toFixed(1)}</span>
        )}
        {restaurant.userRatingsTotal != null && (
          <span className="reviews">({restaurant.userRatingsTotal})</span>
        )}
        <span className={`status ${restaurant.isOpen ? 'open' : 'closed'}`}>
          {restaurant.isOpen ? 'Open' : 'Closed'}
        </span>
      </div>
      <div className="chips">
        {restaurant.categories.map((c) => (
          <span key={c} className="chip chip--sm">{c}</span>
        ))}
      </div>
    </div>
  )
}
