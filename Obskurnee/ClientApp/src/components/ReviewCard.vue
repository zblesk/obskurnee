<template>
  <div>
      <div>
        {{ review.rating }}
      </div>
      <div>
        <p v-html="review.renderedReviewText"></p>
      </div>
  </div> 
</template>

<script>
export default {
    name: "ReviewCard",
    props: ['review'],
}
</script>

