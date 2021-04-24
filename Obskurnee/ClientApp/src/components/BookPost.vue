<template>
    <div  class="book">
      <div class="book__top">
        <div class="book__cover" v-if="post.imageUrl">
          <a :href="post.url" class="book__link">
            <img :src="post.imageUrl" :alt="post.title">
          </a>
        </div>
        <a :href="post.url" class="book__link">
          <h2 class="book__title">{{ post.title }}</h2>
        </a> 
        <p v-if="post.author" class="book__author">{{ post.author }}</p>
        <p class="book__pages" v-if="post.pageCount">{{ post.pageCount }} strán</p>
        <p class="book__owner">Navrhla {{ post.ownerName }}</p>
        <div class="book__text" v-html="post.renderedText"> </div>
      </div>
      <div class="book__repost" v-if="isRepostingAllowed && activeDiscussionId != post.discussionId">
        <router-link v-if="isRepostingAllowed && activeDiscussionId != post.discussionId" 
          :to="{ name: 'discussion', params: { discussionId: activeDiscussionId }, 
          query: { parentPostId: post.postId, fromDiscussionId: post.discussionId } }">
            <button class="button-secondary button-repost">Přidej do aktuálního hlasování</button>
        </router-link>
      </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
    export default {
        name: "BookPost",
        props: ['post'],
        computed: {
          ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"]),
        }
    }
</script>