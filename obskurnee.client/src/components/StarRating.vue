<template>
  <div class="stars" :title="`${rating.toFixed(1)} / ${max}`" role="img"
       :aria-label="`${rating.toFixed(1)} out of ${max} stars`">
    <div class="stars__empty">{{ '★'.repeat(max) }}</div>
    <div class="stars__filled" :style="{ width: fillPercent + '%' }">{{ '★'.repeat(max) }}</div>
  </div>
</template>

<script>
export default {
  name: 'StarRating',
  props: {
    rating: { type: Number, required: true },
    max: { type: Number, default: 5 }
  },
  computed: {
    fillPercent() {
      return Math.max(0, Math.min(100, (this.rating / this.max) * 100));
    }
  }
}
</script>

<style scoped>
.stars {
  position: relative;
  display: inline-block;
  font-size: 1.25rem;
  line-height: 1;
  white-space: nowrap;
}

.stars__empty {
  color: var(--c-border, #ccc);
}

.stars__filled {
  position: absolute;
  top: 0;
  left: 0;
  overflow: hidden;
  color: gold;
  white-space: nowrap;
  pointer-events: none;
}
</style>
