export function CategoryFilter({ categories, selected, onChange }) {
  const toggle = (cat) =>
    onChange(
      selected.includes(cat)
        ? selected.filter((c) => c !== cat)
        : [...selected, cat]
    )

  return (
    <div className="category-filter">
      {categories.map((cat) => (
        <button
          key={cat}
          className={`chip ${selected.includes(cat) ? 'chip--active' : ''}`}
          onClick={() => toggle(cat)}
          type="button"
        >
          {cat}
        </button>
      ))}
    </div>
  )
}
