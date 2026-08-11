<template>
  <div v-if="rating > 4"
      title="5 / 5" role="img"
       aria-label="`5 out of 5 stars`"
       class="stars">
      🌟🌟🌟🌟🌟
  </div>
  <div v-else class="stars" :title="`${rating.toFixed(1)} / 5`" role="img"
       :aria-label="`${rating.toFixed(1)} out of 5 stars`">
    <div class="stars__empty">{{ '★'.repeat(5) }}</div>
    <div class="stars__filled" :style="{ width: fillPercent + '%' }">{{ '★'.repeat(5) }}</div>
  </div>
</template>

<script>
export default {
  name: 'StarRating',
  props: {
    rating: { type: Number, required: true },
  },
  computed: {
    fillPercent() {
      return Math.max(0, Math.min(100, (this.rating / 5.0) * 100));
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
