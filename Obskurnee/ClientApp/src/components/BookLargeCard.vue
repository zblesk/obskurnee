<template>
 <div class="card">
  <div class="row no-gutters">
    <div class="col-sm-5">
        <img class="card-img pr" :src="post.imageUrl" :alt="post.title" v-if="post.imageUrl">
    </div>
    <div class="col-sm-7">
      <div class="card-body">
        <slot></slot>
        <h2 class="card-title">{{ post.title }} - {{ post.author }}</h2>
        <p class="card-text" v-html="post.renderedText"></p>
        
      </div>
    </div>
  </div>
</div> 
</template>


<script>
    export default {
        name: "BookLargeCard",
        props: ['post']
    }
</script>

<style scoped>
div {
  color: salmon;
}

.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  max-width:  800px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
}

.container {
  padding: 2px 16px;
}

.pr { 
  padding: 1ex;
}
</style>

