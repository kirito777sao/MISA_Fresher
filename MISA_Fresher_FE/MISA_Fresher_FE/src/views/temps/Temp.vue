<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()
const goHome = () => router.push('/')
</script>

<template>
  <div class="dev-container">
    <div class="dev-content">
      <div class="icon">Tính năng đang được xây dựng</div>
      <h1 class="title">Chúng tôi đang phát triển!</h1>
      <p class="message">
        Xin lỗi vì sự bất tiện này. <br />
        Tính năng này hiện đang được phát triển.<br />
        Đội ngũ của chúng tôi đang nỗ lực để mang đến cho bạn sớm nhất.
      </p>

      <div class="progress-bar">
        <div class="progress-fill"></div>
      </div>
      <p class="progress-text">Sắp ra mắt...</p>

      <button @click="goHome" class="btn-home">Quay lại Trang chủ</button>
    </div>
  </div>
</template>

<style scoped>
.dev-container {
  min-height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8fafc;
}

.dev-content {
  text-align: center;
  max-width: 100%;
  padding: 2rem;
}

.icon {
  font-size: 5rem;
  margin-bottom: 1rem;
  background: transparent;
}

.title {
  font-size: 2.8rem;
  color: #1e293b;
  margin: 1rem 0;
}

.message {
  font-size: 1.2rem;
  color: #64748b;
  line-height: 1.7;
  margin-bottom: 3rem;
}

.progress-bar {
  width: 100%;
  height: 12px;
  background: #e2e8f0;
  border-radius: 50px;
  overflow: hidden;
  margin-bottom: 1rem;
}

.progress-fill {
  width: 75%;
  height: 100%;
  background: linear-gradient(90deg, #07968f, rgb(26, 163, 14));
  border-radius: 50px;
  animation: pulse 2s infinite;
}

.progress-text {
  color: #94a3b8;
  font-style: italic;
  margin-bottom: 2rem;
}

.btn-home {
  padding: 1rem 2.5rem;
  font-size: 1.1rem;
  background: #009b71;
  color: white;
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-home:hover {
  background: #007b5d;
  transform: translateY(-3px);
  box-shadow: 0 10px 20px rgba(59, 130, 246, 0.3);
}

@keyframes pulse {
  0% {
    width: 70%;
  }
  50% {
    width: 80%;
  }
  100% {
    width: 70%;
  }
}
</style>
