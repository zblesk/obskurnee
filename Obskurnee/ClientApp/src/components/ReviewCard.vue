<template>
  <div>
      <div>
        <img :src="review.imageUrl" :alt="review.bookTitle" v-if="review.imageUrl">
      </div>
      <div>
        <p v-html="review.renderedReviewText"></p>
        <p>{{ review }} </p>
      </div>
  </div> 
</template>

<script>
export default {
    name: "ReviewCard",
    props: ['review'],
}
</script>

