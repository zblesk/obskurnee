<template>
  <div>
      <div>
        {{ review.rating }}
      </div>
      <div>
        <p v-html="review.renderedReviewText"></p>
      </div>

<div >
<hr />
      Vsetky moznosti: 
      {{ review }}
</div>

  </div> 
</template>

<script>
export default {
    name: "UsersReviewCard",
    props: ['review'],
}
</script>

