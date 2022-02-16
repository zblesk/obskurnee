<template>
<div class="form-field u-mb-0">
    <div class="label-md-wrapper">
    <label for="text">{{$t('newpost.comment')}}<span v-if="required">*</span></label>
    <div class="mo-md">
        <div class="mo-md-pic">
          <img src="../assets/Markdown-mark.svg" alt="null">
        </div>
        <div class="mo-md-link">
          <markdown-help-link tabindex="7"></markdown-help-link>
        </div>
        <div class="mo-preview">
          <button class="button-secondary button-small" tabindex="9" @click="togglePreview">{{ $t('global.preview') }}</button>
        </div>
    </div>
    </div>
    <div v-if="previewMode" class="preview">
      <div class="book__text" v-html="previewHtml"></div>
    </div>
    <textarea v-else 
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      id="text" 
      :required="required" 
      :placeholder="$t('global.markdownSamplePlaceholder')" 
      tabindex="5"></textarea>
</div>
</template>

<script>
import axios from "axios";
import MarkdownHelpLink from "./MarkdownHelpLink.vue";
export default {
  name: "MarkdownEditor",
  components: { MarkdownHelpLink },
  props: {
    modelValue: {
      type: String,
      required: false,
    },
    required: {
        type: Boolean,
        required: false,
        default: false
    },
  },
  emits: ['update:modelValue'],
  data() {
    return {
      previewMode: false,
      previewHtml: ""
    };
  },
  methods: {
    async togglePreview() {
      this.previewMode = !this.previewMode;
      if (this.previewMode)
      {
        let preview = await axios.put("/api/discussions/preview", { text: this.modelValue });
        console.log(preview.data);
        this.previewHtml = preview.data.html;
      }
    }
  }
}
</script>

<style scoped>
.mo-preview {
  font-size: 0.875rem;
  margin: 0 0 0 calc(var(--spacer) / 2);
  font-weight: normal;
}

.mo-preview button {
  padding: 0.3em 0.6em;
}

.preview {
  background-color: #f5f5f5;
  padding: 1ex;
}
</style>
